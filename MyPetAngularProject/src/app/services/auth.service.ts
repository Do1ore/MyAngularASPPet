import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {catchError, map, Observable, of} from 'rxjs';
import {environment} from "../../environments/environment";
import {User} from "../models/user";
import {Subject} from "rxjs";
import {SignalRMessageService} from "./signal-r-message.service";
import {waitForAsync} from "@angular/core/testing";


@Injectable({
  providedIn: 'root'
})

export class AuthService {
  apiUrl: string = environment.baseApiUrl + "api/" + 'Auth';
  token!: string;

  public logoutSubject = new Subject<void>();
  public logout$ = this.logoutSubject.asObservable();

  public loginSubject = new Subject<void>();
  public login$ = this.loginSubject.asObservable();
  private currentUserEmailSubject = new Subject<string>();
  public userEmail$ = this.currentUserEmailSubject.asObservable();

  constructor(private http: HttpClient, public signalService: SignalRMessageService) {
  }

  public register(user: User): Observable<any> {
    return this.http.post<any>(
      this.apiUrl + '/register',
      user
    );
  }

  public login(user: User) : Observable<string> {
    const headers = new HttpHeaders()
      .set('Content-Type', 'application/json');

    return this.http.post<string>(this.apiUrl + '/login', user, {
      headers: headers,
      withCredentials: true,
    });
  }

  public getMe(): Observable<any> {
    this.http.get(this.apiUrl + '/getme', {
      responseType: 'text',
    }).subscribe((response) => {
      this.currentUserEmailSubject.next(response);
    });
    return this.userEmail$;
  }


  public isAuthorized(): Observable<boolean> {

    return this.http.get(this.apiUrl + '/refresh-token', {
      responseType: 'json',
      withCredentials: true,
    }).pipe(
      map((response) => {
        console.log(response);
        return true;
      }),
      catchError((error) => {
        console.log(`error status : ${error.status} ${error.message}`);

        switch (error.status) {
          case 401: {
            console.log('Unauthorized');
            return of(false);
          }
          case 403: {
            console.log('Forbidden');
            return of(false);
          }
          default: {
            return of(false);
          }
        }
      })
    );
  }


  public logOut(): void {
    localStorage.removeItem(environment.authTokenName);
    this.logoutSubject.next();
    this.signalService.disconnectFromHub();
  }
}
