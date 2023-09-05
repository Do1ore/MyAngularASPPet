import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {ChatMainModel} from 'src/app/models/chatMainModel';
import {SignalRMessageService} from "../../services/signalR/signal-r-message.service";
import {Subscription} from "rxjs";
import {Dropdown, DropdownInterface, DropdownOptions, Modal, ModalInterface, ModalOptions} from "flowbite";
import {AppUser} from "../../models/appUser";
import {ToastrService} from "ngx-toastr";
import {CreateChatDto} from "../../models/createChatDto";
import {AuthService} from "../../services/auth.service";
import {UserService} from "../../services/user.service";
import {SignalRChatService} from "../../services/signalR/signal-r-chat.service";
import {SignalRConnectionService} from "../../services/signalR/signalr-connection.service";
import {LocalStorageHelperService} from "../../services/local-storage-helper.service";

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {

  @Output() chatSelected: EventEmitter<string> = new EventEmitter<string>();
  public chatMainModel: ChatMainModel[] = [];
  public appUsers: AppUser[] = [];
  public appUsersSearch: AppUser[] = [];
  public usersToAdd: AppUser[] = [];
  public searchTerm: string = '';
  private dropdown: DropdownInterface = new Dropdown();


  public chatName: string = '';
  public fileName: string = '';
  private chatImageInput: File | null = null;

  constructor(
    public signalRConnectionService: SignalRConnectionService,
    public signalRMessageService: SignalRMessageService,
    public userService: UserService,
    public toaster: ToastrService,
    public authService: AuthService,
    public chatService: SignalRChatService,
    private storageHelper: LocalStorageHelperService,
  ) {
  }

  async ngOnInit(): Promise<void> {
    this.authService.logout$.subscribe(() => {
      this.chatMainModel = [];
      this.appUsers = [];
      this.appUsersSearch = [];
      this.signalRConnectionService.disconnectFromHub();
      console.log('Logging out');
      return;
    });

    if (!this.signalRConnectionService.isConnected()) {
      console.log('Connection started - 1')
      await this.signalRConnectionService.waitForConnection();
      console.log('Actual status: ', this.signalRConnectionService.connectionState)
      console.log('Connected - 2')
    }
    console.log('Next line - 3')

    this.initDropdownMenu();

    await this.getChatInfo();

    this.signalRMessageService.onReceiveLastMessage((chatId, message) => {
      this.chatMainModel.forEach(a => {
        if (a.id === chatId) a.lastMessage = message;
      })
    })
    //delete chat listener
    this.chatService.deleteChatListener((chatId) => {
      this.chatMainModel.forEach((chatModel) => {
        if (chatId === chatModel.id) {
          const index = this.chatMainModel.findIndex(obj => obj.id === chatId);
          if (index !== -1) {
            this.chatMainModel.splice(index, 1);
          }
        }
      })
    })

  }

  selectChat(chatId: string) {
    this.chatSelected.emit(chatId);
    this.chatService.currentChatId = chatId;
  }


  addUserToArray(userId: string) {
    console.log('Attempt to add user with id: ', userId)
    let user = this.appUsers.find(a => a.id === userId)!;
    if (!user) {
      this.toaster.info("User not found", "Error");
      return;
    }
    if (this.usersToAdd.find(a => a.id === userId)) {
      this.toaster.info("User that you want to add to a chat is already added", "User already added");
      return;
    }
    this.usersToAdd.push(user)
  }

  chatModalCreate(): ModalInterface {
    const $modalElement: HTMLElement | null = document.querySelector('#add-chat-modal');

    const modalOptions: ModalOptions = {
      placement: 'top-center',
      backdrop: 'dynamic',
      backdropClasses: '',
      closable: true,
      onHide: () => {
        console.log('modal is hidden');
        this.hideDropdown();
      },
      onShow: () => {
        console.log('modal is shown');
      },
      onToggle: () => {
        console.log('modal has been toggled');
      }
    }
    return new Modal($modalElement, modalOptions);
  }

  createChat() {
    let chatDto: CreateChatDto = new CreateChatDto();
    chatDto.chatName = this.chatName;
    if (this.usersToAdd.length <= 0) {
      this.toaster.warning("You want to create chat without users", "No users")
      return;
    }
    console.log('Users to in create function:', this.usersToAdd)
    this.usersToAdd.forEach(u => chatDto.userIds.push(u.id));

    //current userId
    let chatAdministratorUserId = this.storageHelper.getUserIdFromToken();
    chatDto.creatorId = chatAdministratorUserId;
    //add chat administrator to array of users
    chatDto.userIds.push(chatAdministratorUserId);
    chatDto.chatImage = this.chatImageInput;

    this.chatService.createChatCaller(chatDto);
    this.chatService.createChatListener((chat) => {
        this.chatMainModel.push(chat);
        this.chatService.joinChat(chat.id);
        this.chatModalCreate().hide();
      }
    );
  }

  showCreateChatModal() {
    if (this.chatModalCreate()) {
      let modal = this.chatModalCreate();
      modal.show();
    }
  }

  hideCreateChatModal() {
    if (this.chatModalCreate()) {
      let modal = this.chatModalCreate();
      modal.hide();
    }
  }

  initDropdownMenu() {
    const $targetEl: HTMLElement | null = document.getElementById('dropdownUsers');

    const $triggerEl: HTMLElement | null = document.getElementById('default-search');
    // options with default values
    const options: DropdownOptions = {
      placement: 'bottom',
      triggerType: 'click',
      offsetSkidding: 0,
      offsetDistance: 10,
      delay: 100,
      onHide: () => {
        console.log('dropdown has been hidden');
      },
      onShow: () => {
        console.log('dropdown has been shown');
      },
      onToggle: () => {
        console.log('dropdown has been toggled');
      }
    };

    if (!$targetEl && !$triggerEl) {
      return;
    }
    this.dropdown = new Dropdown($targetEl, $triggerEl, options);
  }

  showDropdown() {
    this.dropdown.show();
  }

  hideDropdown() {
    this.dropdown.hide();
  }

  async getChatInfo(): Promise<void> {
    this.chatService.getAllChatsForUserCaller();
    this.chatService.getAllChatsForUserListener((chats) => {
      this.chatMainModel = chats;
      console.log('Connected to chat/s:', this.chatMainModel)
    });
    return;
  }

  searchUsers(): void {
    this.appUsersSearch = [];
    if (this.searchTerm.length <= 0) {
      return;
    }
    this.userService.searchUser(this.searchTerm).subscribe((model: AppUser[]) => {
      this.appUsers = model;
      this.appUsersSearch = model;
      console.log(model.length);
      if (model.length <= 0) {
        console.log('No users found')
        return;
      }
      model.forEach((u) => {
          model.find(a => a.id === u.id)!.imageURL = 'https://e7.pngegg.com/pngimages/436/585/png-clipart-computer-icons-user-account-graphics-account-icon-vector-icons-silhouette.png';
        }
      );
    })
  }

  joinGroups() {
    if (this.chatMainModel.length > 0) {
      this.chatMainModel.forEach((a) => {
        this.chatService.joinChat(a.id);
      })
    }
  }

  onFileSelected({event}: { event: any }) {
    const file: File = event.target.files[0];

    if (file) {
      this.fileName = file.name;
      this.chatImageInput = file;
    }
  }
}
