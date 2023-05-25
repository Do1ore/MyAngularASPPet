import {Component, OnInit} from '@angular/core';
import {SignalRMessageService} from "../../services/signal-r-message.service";
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements  OnInit{
  private hubConnection: HubConnection | undefined;
  private baseApiUrl = environment.baseApiUrl;

  constructor(
    private signalRMessageService : SignalRMessageService
  ) {}

  ngOnInit(): void {
    this.startConnection();
  }
  public startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.baseApiUrl + 'hub/user')
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('Error while starting SignalR connection:', err));
  }

  // Реализуйте методы для получения сообщений и чатов через SignalR.
  // Методы будут вызываться компонентами Angular для подписки на обновления.

  // Пример метода для получения всех сообщений пользователя
  public getAllMessagesForUser(userId: string) {
    if (this.hubConnection === undefined)
      return;

    return this.hubConnection.invoke('GetAllMessagesForUser', userId);
  }

  // Пример метода для получения всех чатов пользователя
  public getAllChatsForUser(userId: string) {
    if (this.hubConnection === undefined)
      return;
    return this.hubConnection.invoke('GetAllChatsForUser', userId);
  }
}
