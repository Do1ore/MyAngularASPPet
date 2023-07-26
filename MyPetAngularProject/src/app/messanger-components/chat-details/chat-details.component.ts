import {AfterViewInit, Component, ElementRef, Input, OnChanges, OnInit, ViewChild} from '@angular/core';
import {SignalRMessageService} from "../../services/signal-r-message.service";
import {ChatMainModel} from "../../models/chatMainModel";
import {ChatMessage} from "../../models/chatMessage";
import {UserProfileService} from "../../services/user-profile.service";
import {Modal, ModalOptions} from "flowbite";
import {AuthService} from "../../services/auth.service";
import {ToastrService} from "ngx-toastr";
import {logMessages} from "@angular-devkit/build-angular/src/builders/browser-esbuild/esbuild";
import {Subscription} from "rxjs";
import {AppUser} from "../../models/appUser";

@Component({
  selector: 'app-chat-details',
  templateUrl: './chat-details.component.html',
  styleUrls: ['./chat-details.component.scss']
})
export class ChatDetailsComponent implements OnInit, OnChanges, AfterViewInit {

  chatModel: ChatMainModel = new ChatMainModel();
  @Input() chatId: string = '';
  public userId = '';
  message: string = '';
  private modalInterface: Modal | null = null;
  private subscription: Subscription = new (Subscription);
  isInitialized: boolean = false;

  constructor(
    public signalRMessageService: SignalRMessageService,
    public userProfileService: UserProfileService,
    public authService: AuthService,
    public toaster: ToastrService) {
  }

  @ViewChild('bottom') scrollTarget!: ElementRef;

  ngAfterViewInit() {
    setTimeout(() => {
      this.scrollToBottom();
    }, 200);
  }

  scrollToBottom() {
    if (this.scrollTarget && this.scrollTarget.nativeElement) {
      this.scrollTarget.nativeElement.scrollIntoView({behavior: 'smooth'});
    }
  }

  async ngOnInit() {
    this.authService.logout$.subscribe(() => {
      this.userId = '';
      console.log('Logging out');
      return;
    });
    await this.initializeChat();
    this.isInitialized = true;

  }

  public deleteChat() {
    this.chatId = "";
    this.signalRMessageService.deleteChatCaller(this.chatModel.id);
    this.closeModal();
  }

  async initializeChat() {
    this.userId = this.signalRMessageService.getUserIdFromToken();
    console.log('id: ' + this.userId)
    console.log("from this: " + this.chatId);

    this.subscription = this.signalRMessageService.signalRConnect$.subscribe(async () => {
      console.log('Connection started');
      await this.getChatModel();


      this.signalRMessageService.onReceiveMessage((message: ChatMessage) => {
        let user = this.chatModel.appUsers.find(a => a.id === message.senderId);
        if (user !== undefined) {
          message.senderInformation = user;
        }
        this.chatModel.messages.push(message);
        this.scrollToBottom();
        console.log('Received new message:', message);
        console.log('Chat model: ', this.chatModel)
        console.log('Current userid: ', this.userId)
      });

      this.signalRMessageService.deleteChatListener((chatId) => {
        this.toaster.info('Chat deleted', 'Chat ' + this.chatModel.name + 'deleted. You automatically disconnected');
        if (this.chatModel.id == chatId) {
          this.chatModel = new ChatMainModel();
        }
      })
    })

  }

  async sendMessage() {
    if (this.message === '') {
      console.log('message null')
      return;
    }
    if (this.signalRMessageService.getHubConnection().state !== 'Connected') {
      this.toaster.error("Not connected to hub", "Server issue");
      return;
    }
    console.log('Sending message...')
    this.signalRMessageService.sendMessage(this.chatId, this.message);
    console.log(this.message);
    this.message = '';
    this.scrollToBottom();
  }


  async getChatModel() {
    if (this.signalRMessageService.getHubConnection().state !== 'Connected') {
      this.toaster.error("Not connected to hub", "Server issue");
      return;
    }
    if (this.chatId !== "") {
      this.signalRMessageService.getChatDetailsCaller(this.chatId);
      this.signalRMessageService.getChatDetailsListener();
      this.signalRMessageService.chatDetailsSubject.asObservable().subscribe((model) => {
        this.chatModel = model;
      });
    }
  }


  ngOnChanges() {
    if (this.chatId !== '') {
      this.getChatModel().then(() => {
        console.log(this.chatId);
      });
    }
    this.subscription.unsubscribe();
  }

  initModal(): Modal {
    const $modalElement: HTMLElement = document.querySelector('#popup-modal')!;

    const modalOptions: ModalOptions = {
      placement: 'top-center',
      backdrop: 'dynamic',
      backdropClasses: 'bg-gray-900 bg-opacity-50 dark:bg-opacity-80 fixed inset-0 z-40',
      closable: true,
      onHide: () => {
        console.log('modal is hidden');
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

  toggleDeleteModal() {
    let modal = new Modal();
    if (!this.modalInterface) {
      modal = this.initModal();

    }
    modal.toggle();
  }

  getUserBySenderId(userId: string): AppUser {
    let result = this.chatModel.appUsers.find(u => u.id === userId);
    if (result === undefined) {
      console.error('Undefined user');
      return new AppUser();
    }
    return result;
  }

  closeModal() {
    let modal = new Modal();

    if (!this.modalInterface) {
      modal = this.initModal();
    }
    modal.hide()
  }
}
