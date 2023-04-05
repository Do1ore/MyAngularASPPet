import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {catchError, Observable, throwError} from 'rxjs';
import {tap} from 'rxjs/operators';
import {environment} from "../../environments/environment";
import {User} from "../models/user";


@Injectable({
  providedIn: 'root'
})

export class AuthService {
  apiUrl: string = environment.baseApiUrl + "api/" + 'Auth';
  token!: string;

  constructor(private http: HttpClient) {
  }

  public register(user: User): Observable<any> {
    return this.http.post<any>(
      this.apiUrl + '/register',
      user
    );
  }

  public login(user: User): Observable<string> {
    return this.http.post(this.apiUrl + '/login', user, {
      responseType: 'text',
    });
  }

  public getMe(): Observable<string> {
    return this.http.get(this.apiUrl, {
      responseType: 'text',
    });
  }
}
