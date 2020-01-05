import { AlertifyService } from './../_services/alertify.service';
import { AuthService } from './../_services/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService,
              private alertifyService: AlertifyService,
              private route: Router) {

  }

  canActivate(): boolean {
    if (this.authService.loggedin()) {
      return true;
    }

    this.alertifyService.error('You are not the one!');
    this.route.navigate(['/home']);
    return false;
  }
}
