import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegistration = new EventEmitter();
  model: any = {};

  constructor(private auth: AuthService, private alertify : AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.auth.register(this.model).subscribe(
      () => {
        this.alertify.success('registration successful');
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  cancel() {
    this.cancelRegistration.emit(false);
    this.alertify.message('cancelled');
  }

}
