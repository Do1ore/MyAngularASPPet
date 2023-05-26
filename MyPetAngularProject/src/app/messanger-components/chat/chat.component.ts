import {Component, OnInit} from '@angular/core';
import {Chat} from 'src/app/models/chat';
import {SignalRMessageService} from "../../services/signal-r-message.service";

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  constructor(
    private signalRMessageService: SignalRMessageService) {
  }
  // @ts-ignore
  public chat: Chat;

  ngOnInit(): void {
    this.signalRMessageService.getHubConnection();
  }

  getChatInfo() : void{
    this.signalRMessageService.getAllChatsForUserCaller();
    this.signalRMessageService.getAllChatsForUserListener();
    this.chat = this.signalRMessageService.chatModel;

  }


}
