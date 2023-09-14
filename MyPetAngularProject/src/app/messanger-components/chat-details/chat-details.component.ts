import {AfterViewInit, Component, ElementRef, Input, OnChanges, OnInit, ViewChild} from '@angular/core';
import {SignalRMessageService} from "../../services/signalR/signal-r-message.service";
import {ChatMainModel} from "../../models/chatMainModel";
import {ChatMessage} from "../../models/chatMessage";
import {ChatImageService} from "../../services/image/chat-image.service";
import {Modal} from "flowbite";
import {AuthService} from "../../services/auth.service";
import {ToastrService} from "ngx-toastr";
import {Subscription} from "rxjs";
import {AppUser} from "../../models/appUser";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";
import {ModalHelperService} from "../../services/Ui/modal-helper.service";
import {UserImageService} from "../../services/image/user-image.service";
import {LocalStorageHelperService} from "../../services/local-storage-helper.service";
import {SignalRConnectionService} from "../../services/signalR/signalr-connection.service";
import {SignalRChatService} from "../../services/signalR/signal-r-chat.service";
import {SignalRUserService} from "../../services/signalR/signal-r-user.service";


@Component({
  selector: 'app-chat-details',
  templateUrl: './chat-details.component.html',
  styleUrls: ['./chat-details.component.scss']
})
export class ChatDetailsComponent implements OnInit, OnChanges, AfterViewInit {


  @Input() chatId: string = '';
  public userId = '';
  message: string = '';

  isInitialized: boolean = false;
  safeChatImgProfileUrl: SafeUrl = "";
  //All chat data, like messages, users
  public chatModel: ChatMainModel = new ChatMainModel();

  private deleteChatModalInterface: Modal | null = null;
  private editChatModalInterface: Modal | null = null;
  private subscription: Subscription = new Subscription;
  private readonly deleteModalId: string = '#popup-modal';
  public readonly editModalId: string = '#edit-popup-modal';

  constructor(
    public sanitizer: DomSanitizer,
    public imageService: ChatImageService,
    public authService: AuthService,
    public toaster: ToastrService,
    public modalHelper: ModalHelperService,
    public userImageService: UserImageService,
    public chatService: SignalRChatService,
    public messageService: SignalRMessageService,
    private storageHelper: LocalStorageHelperService,
    private connectionService: SignalRConnectionService,
    private signalRUserService: SignalRUserService,
  ) {
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

    if (!this.connectionService.isConnected()) {
      console.log('Connection started - 1')
      await this.connectionService.waitForConnection();
      console.log('Actual status: ', this.connectionService.connectionState)
      console.log('Connected - 2')
    }
    console.log('Next line - 3')

    await this.initializeChat();
    this.isInitialized = true;

  }

  public deleteChat() {
    this.chatService.deleteChatCaller(this.chatId);
    this.closeDeleteModal();
    console.log('Delete chat called');
  }

  async initializeChat() {
    this.userId = this.storageHelper.getUserIdFromToken();
    console.log('id: ' + this.userId)
    console.log("from this: " + this.chatId);

    await this.getChatModel();
    this.startListeners();
  }

  private startListeners() {
    this.messageService.onReceiveMessage((message: ChatMessage) => {
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

    this.chatService.deleteChatListener((chatId) => {
      this.toaster.info('Chat deleted', 'Chat ' + this.chatModel.name + 'deleted. You automatically disconnected');
      if (this.chatModel.id == chatId) {
        this.chatModel = new ChatMainModel();
      }
    });

    this.signalRUserService.listenUserProfileImageUpdate((fileUrl, userId) => {
      this.userImageService.updateUserProfileImageUrl(fileUrl, userId);
      console.log('User image for user: ', userId, 'updated: ', fileUrl);
    });
  }


  async sendMessage() {
    if (this.message === '') {
      console.log('message null')
      return;
    }
    if (!this.connectionService.isConnected()) {
      this.toaster.error("Not connected to hub", "Server issue");
      return;
    }
    console.log('Sending message...')
    this.messageService.sendMessage(this.chatId, this.message);
    console.log(this.message);
    this.message = '';
    this.scrollToBottom();
  }

  async getChatModel() {
    if (!this.connectionService.isConnected()) {
      this.toaster.error("Not connected to hub", "Server issue");
      return;
    }
    if (this.chatId === "") {
      console.error('Chat id is empty')
      return;
    }
    this.chatService.getChatDetailsCaller(this.chatId);
    this.chatService.getChatDetailsListener(async (chats) => {
      this.chatModel = chats;
      await this.getUserProfileImages();
    });

    console.log('getChatModel started')
    this.getChatProfileImage();

  }


  getChatProfileImage() {
    console.log('Image for chat load...')
    this.imageService.getChatImage(this.chatId).subscribe((image: Blob) => {
      let unsafeUrl = URL.createObjectURL(image);
      this.safeChatImgProfileUrl = this.sanitizer.bypassSecurityTrustUrl(unsafeUrl);
    });
  }

  async getUserProfileImages() {
    if (this.chatModel.appUsers.length <= 0) {
      console.log('Zero users')
      return;
    }

    await this.userImageService.getUserProfileImages(this.chatModel.appUsers);
  }

  ngOnChanges() {
    if (this.chatId !== '') {
      this.getChatModel().then(() => {
        console.log(this.chatId);
      });
    }
    this.subscription.unsubscribe();
  }


  toggleDeleteModal() {
    let modal = new Modal();
    if (!this.deleteChatModalInterface) {
      modal = this.modalHelper.initializeModal(this.deleteModalId);

    }
    modal.toggle();
  }

  getUserBySenderId(userId: string):
    AppUser {
    let result = this.chatModel.appUsers.find(u => u.id === userId);
    if (result === undefined) {
      console.error('Undefined user');
      return new AppUser();
    }
    return result;
  }

  closeDeleteModal() {
    let modal = new Modal();

    if (!this.deleteChatModalInterface) {
      modal = this.modalHelper.initializeModal(this.deleteModalId);
    }
    modal.hide()
  }

  toggleEditModal() {
    let modal = new Modal();
    if (!this.editChatModalInterface) {
      modal = this.modalHelper.initializeModal(this.editModalId);
      modal.toggle();
    }
  }

  getUserCredentials(userId: string) {
    let userData = this.chatModel.appUsers.find(a => a.id == userId);
    if (!userData) {
      return 'ML'
    }
    if (!userData.surname) {
      return userData.username.substring(0, 2).toUpperCase();
    }
    return userData.surname.substring(0, 1).toUpperCase() + userData.username.substring(0, 1).toUpperCase();
  }
}
