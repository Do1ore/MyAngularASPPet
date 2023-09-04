import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr";
import {environment} from "../../../environments/environment";
import {Subject} from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class SignalRConnectionService {
    public hubConnection: HubConnection | null = null;
    private baseApiUrl = environment.baseApiUrl;
    public connectionState: HubConnectionState = HubConnectionState.Disconnected;


    protected signalRConnectedSubject: Subject<void> = new Subject<void>();

    public async startHubConnection(): Promise<HubConnection> {
        console.log('method started!')
        if (!this.hubConnection || this.hubConnection?.state !== 'Connected') {
            this.hubConnection = new HubConnectionBuilder()
                .withUrl(this.baseApiUrl + 'hub/user')
                .withAutomaticReconnect()
                .build();

            this.hubConnection.start()
                .then(() => {
                    this.connectionState = HubConnectionState.Connected;
                })
                .catch(err => console.error('Error while starting SignalR connection:', err));
        }
        return this.hubConnection;
    }

    isConnected() {
        return (this.hubConnection?.state === HubConnectionState.Connected)
    }

    public disconnectFromHub() {
        if (this.hubConnection) {
            this.hubConnection.stop().then();
            console.log('Connection with hub closed');
        }
    }
}

