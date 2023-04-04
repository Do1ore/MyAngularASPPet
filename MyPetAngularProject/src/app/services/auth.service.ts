import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {tap} from 'rxjs/operators';
import {environment} from "../../environments/environment";

interface LoginResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private readonly apiUrl = environment.baseApiUrl + 'api/' + 'Auth';

  constructor(private http: HttpClient) {
  }

  register(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  login(data: any): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, data).pipe(
      tap(response => {
        if (response && response.token) {
          localStorage.setItem('token', response.token);
        }
      })
    );
  }


  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) {
      return false;
    } else return true;
  }

  logout(): void {
    localStorage.removeItem('token');
  }
}
