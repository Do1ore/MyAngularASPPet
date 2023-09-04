import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {Observable} from "rxjs";
import {SignalRMessageService} from "../signalR/signal-r-message.service";

@Injectable({
  providedIn: 'root'
})
export class ChatImageService {
  baseApiUrl: string = environment.baseApiUrl + "api/" + 'Image';


  constructor(private http: HttpClient, public signalRService: SignalRMessageService) {
  }

  public loadImage(file: FormData): Observable<any> {
    return this.http.post(this.baseApiUrl + '/load-image', file, {
      reportProgress: true,
      observe: 'events',
    });
  }

  public getChatImage(chatId: string): Observable<Blob> {
    const headers = new HttpHeaders().set('Accept', 'image/jpeg');
    const url = this.baseApiUrl + "/get-chat-image/" + chatId;

    return this.http.get(url, {
      headers: headers,
      responseType: 'blob'
    });
  }

}

