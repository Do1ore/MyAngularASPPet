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

  async waitForMessages(): Promise<void> {
    while (this.signalRMessageService.chatModel == null) {
      //wait 100 milliseconds and check state again and again
      await new Promise(resolve => setTimeout(resolve, 100));
    }
  }

  async ngOnInit(): Promise<void> {
    await this.getChatInfo();
  }

  async getChatInfo(): Promise<void> {
    await this.waitForHubConnection();
    this.signalRMessageService.getHubConnection();
    this.signalRMessageService.getAllChatsForUserCaller();
    this.signalRMessageService.getAllChatsForUserListener();
    this.subscription = this.signalRMessageService.modelSubject.asObservable().subscribe((model: ChatMainModel[]) => {
      this.chatMainModel = model;
    })
    await this.waitForMessages();
    console.log(this.chatMainModel)
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
