import {Component, EventEmitter, OnDestroy, OnInit, Output} from '@angular/core';
import {ChatMainModel} from 'src/app/models/chatMainModel';
import {SignalRMessageService} from "../../services/signal-r-message.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnDestroy {
  @Output() chatSelected: EventEmitter<string> = new EventEmitter<string>();

  constructor(
    public signalRMessageService: SignalRMessageService) {
  }


  private subscription: Subscription | undefined;
  public chatMainModel: ChatMainModel[] = [];

  selectChat(chatId: string) {
    this.chatSelected.emit(chatId);
    console.log('emited' + chatId);
  }

  async waitForHubConnection(): Promise<void> {
    while (this.signalRMessageService.getHubConnection().state != 'Connected') {
      //wait 100 milliseconds and check state again and again
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


  async ngOnInit(): Promise<void> {
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
    console.log('Connected to chat/s')

  }

  async getChatInfo(): Promise<void> {
    this.signalRMessageService.getAllChatsForUserCaller();
    this.signalRMessageService.getAllChatsForUserListener();
    this.subscription = this.signalRMessageService.modelSubject.asObservable().subscribe((model: ChatMainModel[]) => {
      this.chatMainModel = model;
    })

    console.log('zxczxczxcxz' + this.chatMainModel)
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

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    this.leftGroups();
    console.log('Disconnected from chat/s')
  }
}
