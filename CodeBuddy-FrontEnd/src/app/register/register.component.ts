import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegistration = new EventEmitter();
  model: any = {};

  constructor(private auth: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.auth.register(this.model).subscribe(
      ()=>{
        console.log('registration successful');
      },
      error =>
      {
        console.log(error);
      }
    );
  }

  cancel() {
    this.cancelRegistration.emit(false);
    console.log('cancelled');
  }

}
