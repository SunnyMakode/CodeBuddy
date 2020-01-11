import { User } from './../_models/User';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

/// Since we are using JwtModule.forRoot() in app.module.ts
/// this code is no longer required
// const httpOptions = {
//   headers: new HttpHeaders({
//     // don't forget that there should be a one letter space after Bearer''
//     Authorization : 'Bearer ' + localStorage.getItem('token')
//   })
// };

@Injectable({
  providedIn: 'root'
})
export class UserService {
baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getUsers(): Observable<User[]> {
  // return this.http.get<User[]>(this.baseUrl + 'user', httpOptions);
  return this.http.get<User[]>(this.baseUrl + 'user');
}

getUser(id): Observable<User> {
  // return this.http.get<User>(this.baseUrl + 'user/' + id, httpOptions);
  return this.http.get<User>(this.baseUrl + 'user/' + id);
}

}
