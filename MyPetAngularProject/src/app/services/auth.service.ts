import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from "../../environments/environment";
import {User} from "../models/user";
import {Subject} from "rxjs";


@Injectable({
  providedIn: 'root'
})

export class AuthService {
  apiUrl: string = environment.baseApiUrl + "api/" + 'Auth';
  token!: string;

  private logoutSubject = new Subject<void>();
  public logout$ = this.logoutSubject.asObservable();
  private currentUserEmailSubject = new Subject<string>();
  public userEmail$ = this.currentUserEmailSubject.asObservable();

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
    this.userEmail$ = this.http.get(this.apiUrl + '/getme', {
      responseType: 'text',
    });
    return this.userEmail$;
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
    this.logoutSubject.next();
  }
}
