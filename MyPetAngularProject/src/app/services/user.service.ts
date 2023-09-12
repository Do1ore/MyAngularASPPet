import {Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {AppUser} from "../models/appUser";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseApiUrl = environment.baseApiUrl + 'api/User';

  constructor(private http: HttpClient) {
  }

  public searchUser(searchTerm: string): Observable<AppUser[]> {
    const payload = {searchTerm: searchTerm};
    return this.http.get<AppUser[]>(this.baseApiUrl + '/search-user' + '/' + searchTerm);
  }
}
