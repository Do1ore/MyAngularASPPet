import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {environment} from "../../environments/environment";
import {Subject} from "rxjs";
import {ChatMainModel} from '../models/chatMainModel';
import {JwtHelperService} from "@auth0/angular-jwt";
import {ToastrService} from "ngx-toastr";

@Injectable({
  providedIn: 'root'
})
export class SignalRMessageService {

  private hubConnection: HubConnection | null = null;
  private baseApiUrl = environment.baseApiUrl;
  private authTokenName = environment.authTokenName;
  public modelSubject: Subject<ChatMainModel[]> = new Subject<ChatMainModel[]>();
  public model$ = this.modelSubject.asObservable();


  public chatModel: ChatMainModel = new ChatMainModel();
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
        .then(() => {
          console.log('SignalR connection started')
          this.getAllChatsForUserListener();
        })
        .catch(err => console.error('Error while starting SignalR connection:', err));
    }
    return this.hubConnection;
  }

  public getAllChatsForUserCaller() {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      console.log("not yet1")
      return;
    }

    console.log("yes1");
    this.hubConnection!.invoke('GetAllChatsForUser', this.userId)
      .catch(err => console.error(err));
  }

  public getAllChatsForUserListener() {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      console.log("not yet1")
      return;
    }
    console.log("yes2")

    this.hubConnection.on('GetAllChatsForUserResponse', (model: ChatMainModel[]) => {
      console.info(model);
      model.forEach((model) => {
        console.log(model.id);
      });
      this.modelSubject.next(model);
    });
  }

  public getAllMessagesForUser() {
    if (this.hubConnection === null)
      return;

    return this.hubConnection.invoke('GetAllMessagesForUser', this.userId);
  }
}
