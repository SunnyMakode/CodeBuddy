import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegistration = new EventEmitter();
  user: User ;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  colorTheme = 'theme-dark-blue';

  constructor(private auth: AuthService,
              private alertify: AlertifyService,
              private fb: FormBuilder,
              private router: Router) { }

  ngOnInit() {

    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });

    this.createRegisterForm();

    /// All these registerForm code can be writtern using Angular provides service called FormBuilder
    /// createRegisterForm() is an example of how we did it
    // this.registerForm = new FormGroup({
    //   username: new FormControl('', Validators.required),
    //   password: new FormControl('', [Validators.required, Validators.maxLength(45), Validators.minLength(5)]),
    //   confirmPassword: new FormControl('', Validators.required)
    // }, this.passwordMatchValidator);
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      password: ['', [Validators.required, Validators.maxLength(15), Validators.minLength(5)]],
      confirmPassword : ['', Validators.required]
    },
    {
      Validators : this.passwordMatchValidator
    });
  }

  passwordMatchValidator(formGroup: FormGroup) {
    return formGroup.get('password').value === formGroup.get('confirmPassword').value
    ? null : {mismatch : true};
  }

  register() {

    if (this.registerForm.valid) {

      this.user = Object.assign({}, this.registerForm.value);

      this.auth.register(this.user).subscribe(() => {
            this.alertify.success('registration successful');
          },
          error => {
            this.alertify.error(error);
          },
          () => {
            this.auth.login(this.user).subscribe(
              () => {
              this.router.navigate(['/members']);
            });
          }

        );
    }

    // this.auth.register(this.model).subscribe(
    //   () => {
    //     this.alertify.success('registration successful');
    //   },
    //   error => {
    //     this.alertify.error(error);
    //   }
    // );
  }

  cancel() {
    this.cancelRegistration.emit(false);
    this.alertify.message('cancelled');
  }

}
