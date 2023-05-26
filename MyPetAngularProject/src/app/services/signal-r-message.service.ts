import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {environment} from "../../environments/environment";
import {Subject} from "rxjs";
import {Chat} from '../models/chat';
import {JwtHelperService} from "@auth0/angular-jwt";
import {ToastrService} from "ngx-toastr";

@Injectable({
  providedIn: 'root'
})
export class SignalRMessageService {
  private hubConnection: HubConnection | undefined;
  private baseApiUrl = environment.baseApiUrl;
  private authTokenName = environment.authTokenName;
  private modelSubject: Subject<Chat> = new Subject<Chat>();
  public model$ = this.modelSubject.asObservable();

  // @ts-ignore
  public chatModel : Chat;
  private readonly userId: string = '';

  constructor(private jwtHelper: JwtHelperService, private toaster: ToastrService) {
    const token = localStorage.getItem(this.authTokenName);
    if (token === null) {
      return;
    }
    const decodedToken = this.jwtHelper.decodeToken(token);
    this.userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
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

  public getAllChatsForUserCaller() {
    if (this.hubConnection === undefined)
      return;

    this.hubConnection.invoke('GetAllChatsForUser', this.userId)
      .catch(err => console.error(err));
  }

  public getAllChatsForUserListener() {
    if (this.hubConnection === undefined) {
      console.error('hub connection undefined');
      return;
    }
    this.hubConnection.on('GetAllChatsForUserResponse', (model: Chat) => {
        console.info(model);
        this.chatModel = model;
      });
  }

  public getAllMessagesForUser() {
    if (this.hubConnection === undefined)
      return;

    return this.hubConnection.invoke('GetAllMessagesForUser', this.userId);
  }
}
