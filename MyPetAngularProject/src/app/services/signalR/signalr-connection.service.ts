import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class SignalRConnectionService {
  public hubConnection: HubConnection | null = null;
  private baseApiUrl = environment.baseApiUrl;
  public connectionState: HubConnectionState = HubConnectionState.Disconnected;


  constructor() {
    this.getHubConnection().then();
  }

  //Connect or get hub connection
  public async getHubConnection(): Promise<HubConnection> {
    console.log('method started!')
    if (!this.hubConnection || this.hubConnection?.state !== 'Connected') {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.baseApiUrl + 'hub/user')
        .withAutomaticReconnect()
        .build();

      this.hubConnection.start()
        .then(() => {
          this.connectionState = HubConnectionState.Connected;
          console.log('Hub connection: ', this.hubConnection);
          console.log('Connection state: ', this.connectionState);
        })
        .catch(err => console.error('Error while starting SignalR connection:', err));
    }
    return this.hubConnection;
  }

  public isConnected() {
    return (this.hubConnection?.state === HubConnectionState.Connected)
  }

  public disconnectFromHub() {
    if (this.hubConnection) {
      this.hubConnection.stop().then();
      console.log('Connection with hub closed');
    }
  }

  async waitForConnection(): Promise<void> {
    return new Promise<void>(async (resolve) => {
      const connection = this.hubConnection!;

      const checkConnectionStatus = () => {
        if (connection.state === HubConnectionState.Connected) {
          resolve();
        } else {
          setTimeout(checkConnectionStatus, 1000); // Проверяем каждую секунду
        }
      };

      checkConnectionStatus();
    });
  }

}

