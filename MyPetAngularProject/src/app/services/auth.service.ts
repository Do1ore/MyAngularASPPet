import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {environment} from "../../environments/environment";
import {User} from "../models/user";
import {error} from "@angular/compiler-cli/src/transformers/util";


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

  public login(user: User): Observable<any> {
    const headers = new HttpHeaders()
      .set('Content-Type', 'application/json');

    return this.http.post(this.apiUrl + '/login', user, {
      headers: headers,
      withCredentials: true,
    });
  }

  public getMe(): Observable<string> {
    return this.http.get(this.apiUrl + '/getme', {
      responseType: 'text',
    });
  }

  public isAuthorized(): boolean {
    this.http.get(this.apiUrl + '/refresh-token', {
      responseType: 'text',
    }).subscribe((response) => {
        console.log(response);
        return true;
      },
      error => {
        console.log(`error status : ${error.status} ${error.message}`);

        switch (error.status) {
          case 401: {
            console.log('Unauthorized')
            return false;
          }
          case 403: {
            console.log('Forbidden')
            return false;
          }
          default: {
            return false;
          }
        }
      });
    if (localStorage.getItem(environment.authTokenName) !== null)
      return true;
    return false;
  }

  public logOut(): void {
    localStorage.removeItem(environment.authTokenName);
  }
}
