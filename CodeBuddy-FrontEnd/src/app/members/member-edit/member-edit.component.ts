import { AuthService } from './../../_services/auth.service';
import { UserService } from './../../_services/user.service';
import { AlertifyService } from './../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/User';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;

  @ViewChild('editForm', {static: true}) editForm: NgForm;

  // @HostListener is used to give popup to prevent user to close the browser when he/she has any pending changes
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute,
              private alertify: AlertifyService,
              private userService: UserService,
              private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(userData => {
        this.user = userData['usersRouteResolver'];
      });
  }

  updateUser() {
    const id = this.authService.decodedToken.nameid;
    this.userService.updateUser(id, this.user).subscribe(next => {
      this.alertify.success('Profile updated successfully');
      this.editForm.reset(this.user);
    },
    err => {
      this.alertify.error(err);
    });

  }

  updateMainPhoto(photoUrlFromOutput) {
    this.user.photoUrl = photoUrlFromOutput;
  }

}
