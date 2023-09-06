import {Injectable} from '@angular/core';

import {ChatMessage} from "../../models/chatMessage";
import {SignalRConnectionService} from "./signalr-connection.service";
import {LocalStorageHelperService} from "../local-storage-helper.service";


@Injectable({
  providedIn: 'root'
})
export class SignalRMessageService {

  constructor(private signalRConnection: SignalRConnectionService, private storageService: LocalStorageHelperService) {
  }

  //MESSAGES//
  public sendMessage(chatId: string, message: string) {
    if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
      return;
    }
    let userId = this.storageService.getUserIdFromToken();

    if (userId.length === 0) {
      return;
    }
    return this.signalRConnection.hubConnection.invoke('SendMessage', chatId, userId, message);
  }

  public onReceiveMessage(callback: (message: ChatMessage) => void): void {
    if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
      return;
    }
    this.signalRConnection.hubConnection.on('ReceiveMessage', (message: ChatMessage) => {
      callback(message);
    });
  }

  public onReceiveLastMessage(callback: (chatId: string, message: string) => void): void {
    if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
      return;
    }
    this.signalRConnection.hubConnection.on('ReceiveLastMessage', (chatId: string, message: string) => {
      callback(chatId, message);
    });

  }
}
