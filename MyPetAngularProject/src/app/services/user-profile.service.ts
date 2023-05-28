import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {
  baseApiUrl: string = environment.baseApiUrl + "api/" + 'Account';
  token!: string;


  constructor(private http: HttpClient) {
  }

  public loadImage(file: FormData): Observable<any> {
    return this.http.post(this.baseApiUrl + '/load-image', file, {
      reportProgress: true,
      observe: 'events',
    });
  }

  public getImage(): Observable<Blob> {
    const headers = new HttpHeaders().set('Accept', 'image/png');

    const url = this.baseApiUrl + "/profile-image";
    return this.http.get(url, {
      headers: headers,
      responseType: 'blob'});
  }
}

