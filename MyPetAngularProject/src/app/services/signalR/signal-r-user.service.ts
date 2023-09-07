import {Injectable} from '@angular/core';
import {SignalRConnectionService} from "./signalr-connection.service";

@Injectable({
  providedIn: 'root'
})
export class SignalRUserService {

  constructor(private readonly signalRConnection: SignalRConnectionService) {
  }

  public listenUserProfileImageUpdate(callback: (fileUrl: string, userId: string) => void) {
    this.signalRConnection.hubConnection?.on('UserProfileImageUploaded', callback);
  }
}
