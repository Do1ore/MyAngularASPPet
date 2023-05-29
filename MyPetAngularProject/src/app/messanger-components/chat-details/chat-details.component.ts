import {Component, Input, OnInit} from '@angular/core';
import {SignalRMessageService} from "../../services/signal-r-message.service";
import {ChatMainModel} from "../../models/chatMainModel";

@Component({
  selector: 'app-chat-details',
  templateUrl: './chat-details.component.html',
  styleUrls: ['./chat-details.component.scss']
})
export class ChatDetailsComponent implements OnInit {

  @Input() chatModel: ChatMainModel = new ChatMainModel();
  @Input() chatId: string = '';
  message: string = '';
  constructor(public signalRMessageService: SignalRMessageService) {
  }
  async waitForHubConnection(): Promise<void> {
    while (this.signalRMessageService.getHubConnection().state != 'Connected') {
      //wait 100 milliseconds and check state again and again
      await new Promise(resolve => setTimeout(resolve, 100));
    }
  }


  ngOnInit() : void {
    console.log("from this: " + this.chatId);
  }

  sendMessage(){
    console.log(this.message);
    this.message = '';
  }
  async getChatModel() {
    await this.waitForHubConnection();
    this.signalRMessageService.getChatDetailsCaller(this.chatId);
    this.signalRMessageService.getChatDetailsListener();
    this.signalRMessageService.chatDetailsSubject.asObservable().subscribe((model) =>
      this.chatModel = model)
    console.log(this.chatModel)
  }

  ngOnChanges() {
    if (this.chatId !== '') {
      this.getChatModel().then(() => {
        console.log(this.chatId);
      });
    }
  }

}
