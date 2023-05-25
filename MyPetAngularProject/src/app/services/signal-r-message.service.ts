import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr'
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})

export class SignalRMessageService {
  private hubConnection: HubConnection | undefined;
  private baseApiUrl = environment.baseApiUrl;

  constructor() {
  }

  public getHubConnection(): HubConnection {
    if (!this.hubConnection) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.baseApiUrl + 'hub/user')
        .build();

      this.hubConnection.start()
        .then(() => console.log('SignalR connection started'))
        .catch(err => console.error('Error while starting SignalR connection:', err));
    }

    return this.hubConnection;
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
