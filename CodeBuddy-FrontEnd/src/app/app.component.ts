import { User } from './_models/User';
import { AuthService } from './_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  jwtHelper = new JwtHelperService();

  constructor(public authService: AuthService) {

  }

  ngOnInit() {
    const userToken = localStorage.getItem('token');
    const user: User = JSON.parse(localStorage.getItem('user'));

    if (userToken) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(userToken);
    }

    if (user) {
      this.authService.currentUser = user;
    }
  }
}
