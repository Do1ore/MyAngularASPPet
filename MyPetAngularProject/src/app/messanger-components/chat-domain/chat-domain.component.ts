import {Component} from '@angular/core';
import {ChatDetailsComponent} from "../chat-details/chat-details.component";

@Component({
  selector: 'app-chat-domain',
  templateUrl: './chat-domain.component.html',
  styleUrls: ['./chat-domain.component.scss']
})
export class ChatDomainComponent {
  selectedChatId: string = '';

  public showChatDetails(chatId: string) {
    this.selectedChatId = chatId;

  }
}
