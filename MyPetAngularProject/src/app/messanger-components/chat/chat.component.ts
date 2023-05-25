import {Component, OnInit} from '@angular/core';
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

  ngOnInit(): void {
    this.signalRMessageService.getHubConnection();
  }
}
