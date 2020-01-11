import { AlertifyService } from '../../_services/alertify.service';
import { UserService } from '../../_services/user.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/User';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  users: User[];

  constructor(private userService: UserService,
              private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(userData => {
      this.users = userData['usersRouteResolver'];
    });
  }

  // loadUsers() {
  //   this.userService.getUsers().subscribe((u: User[]) => {
  //     this.users = u;
  //   }, err => {
  //     this.alertify.error(err);
  //   });
  // }

}
