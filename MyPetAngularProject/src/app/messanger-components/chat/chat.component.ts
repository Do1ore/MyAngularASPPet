import {Component, EventEmitter, OnDestroy, OnInit, Output} from '@angular/core';
import {ChatMainModel} from 'src/app/models/chatMainModel';
import {SignalRMessageService} from "../../services/signal-r-message.service";
import {Subscription} from "rxjs";
import {Dropdown, DropdownInterface, DropdownOptions, Modal, ModalInterface, ModalOptions} from "flowbite";
import {UserProfileService} from "../../services/user-profile.service";
import {AppUser} from "../../models/appUser";
import {DomSanitizer} from "@angular/platform-browser";
import {ToastrService} from "ngx-toastr";
import {CreateChatDto} from "../../models/createChatDto";
import {AuthService} from "../../services/auth.service";
import {Route, Router} from "@angular/router";

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnDestroy {
  get isAuthorized(): boolean {
    return this._isAuthorized;
  }

  set isAuthorized(value: boolean) {
    this._isAuthorized = value;
  }

  private _isAuthorized: boolean = false;

  @Output() chatSelected: EventEmitter<string> = new EventEmitter<string>();
  public chatMainModel: ChatMainModel[] = [];
  public appUsers: AppUser[] = [];
  public appUsersSearch: AppUser[] = [];
  public addUsersArray: AppUser[] = [];
  public isPageIsLoaded: boolean = false;
  public searchTerm: string = '';
  private subscription: Subscription | undefined;
  private dropdown: DropdownInterface = new Dropdown();


  public chatName: string = '';

  constructor(
    public signalRMessageService: SignalRMessageService,
    public userService: UserProfileService,
    private sanitizer: DomSanitizer,
    public toaster: ToastrService,
    public authService: AuthService,
    private router: Router
  ) {
  }

  async ngOnInit(): Promise<void> {
    if (!this.authService.isAuthorized()) {
      this.router.navigate(['/login']).then();
      this.toaster.warning('To use chat you need to be authorized', 'Account required');

      return;
    }
    this.initDropdownMenu();
    this.signalRMessageService.getHubConnection();

    await this.waitForHubConnection();
    await this.getChatInfo();
    await this.waitForChats();

    // Подождите, пока выполнится предыдущая асинхронная операция
    await new Promise<void>((resolve) => {
      setTimeout(() => {
        this.joinGroups();
        resolve();
      }, 0);
    });
    this.signalRMessageService.onReceiveLastMessage((chatId, message) => {
      this.chatMainModel.forEach(a => {
        if (a.id === chatId) a.lastmessage = message;
      })
    })
    console.log('Connected to chat/s')

  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    this.leftGroups();
    console.log('Component destroyed');
    this.signalRMessageService.disconnectFromHub();
  }

  selectChat(chatId: string) {
    this.chatSelected.emit(chatId);
  }

  addUserToArray(userId: string) {
    let user = this.appUsers.find(a => a.id === userId)!;
    if (!user) {
      this.toaster.info("User not found", "Error");

      return;
    }
    if (this.addUsersArray.find(a => a.id === userId)) {
      this.toaster.info("User that you want to add to a chat is already added", "User already added");
      return;
    }
    this.addUsersArray.push(user)
  }

  getNewChatModal(): ModalInterface {
    const $modalElement: HTMLElement | null = document.querySelector('#add-chat-modal');

    const modalOptions: ModalOptions = {
      placement: 'top-center',
      backdrop: 'dynamic',
      backdropClasses: 'bg-gray-900 bg-opacity-50 dark:bg-opacity-80 fixed inset-0 z-40',
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
    if (this.addUsersArray.length <= 0) {
      this.toaster.warning("You want to create chat without users", "No users")
      return;
    }
    this.addUsersArray.forEach(u => chatDto.userId.push(u.id));
    //current userid
    chatDto.userId.push(this.signalRMessageService.getUserIdFromToken());

    this.signalRMessageService.createChatCaller(chatDto);
    this.signalRMessageService.createChatListener((chat) => {
        this.chatMainModel.push(chat)
        this.signalRMessageService.joinChat(chat.id);
      }
    );
  }

  showCreateChatModal() {
    if (this.getNewChatModal()) {
      let modal = this.getNewChatModal();
      modal.show();
    }
  }

  hideCreateChatModal() {
    if (this.getNewChatModal()) {
      let modal = this.getNewChatModal();
      modal.hide();
    }
  }

  initDropdownMenu() {
    const $targetEl: HTMLElement | null = document.getElementById('dropdownUsers');

    const $triggerEl: HTMLElement | null = document.getElementById('default-search')
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

    /*
    * targetEl: required
    * triggerEl: required
    * options: optional
    */
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

  async waitForHubConnection(): Promise<void> {
    while (this.signalRMessageService.getHubConnection().state != 'Connected') {
      //wait 100 milliseconds and check state again and again
      console.log('waiting');
      await new Promise(resolve => setTimeout(resolve, 100));
    }
  }

  async waitForChats(): Promise<void> {
    let model1: ChatMainModel[] = [];
    this.signalRMessageService.chatDetails$.subscribe((model) => model1 = model)

    while (model1.length == 0) {
      console.log(model1.length);

      //wait 100 milliseconds and check state again and again
      await new Promise(resolve => setTimeout(resolve, 100));
    }
  }


  async getChatInfo(): Promise<void> {
    this.signalRMessageService.getAllChatsForUserCaller();
    this.signalRMessageService.getAllChatsForUserListener();
    this.subscription = this.signalRMessageService.modelSubject.asObservable().subscribe((model: ChatMainModel[]) => {
      this.chatMainModel = model;
    })
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
        this.signalRMessageService.joinChat(a.id);
      })
    }
  }

  leftGroups() {
    if (this.chatMainModel.length > 0) {
      this.chatMainModel.forEach((a) => {
        this.signalRMessageService.leaveChat(a.id);
      })
    }
  }


}
