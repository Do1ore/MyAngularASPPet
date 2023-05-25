import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
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
    return this.http.get(this.apiUrl + '/getme', {
      responseType: 'text',
    });
  }

  public isAuthorized(): boolean {
    if (localStorage.getItem(environment.authTokenName) !== null)
      return true;
    return false;
  }

  public logOut(): void {
    localStorage.removeItem(environment.authTokenName);
  }
}
