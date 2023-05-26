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
  public chat: Chat | null = null;

  ngOnInit(): void {
    this.signalRMessageService.getHubConnection();


  }

  getChatInfo() : void{
    this.signalRMessageService.getAllChatsForUserCaller();
    this.signalRMessageService.getAllChatsForUserListener();
    this.signalRMessageService.model$.subscribe((model) => this.chat = model);
  }


}
