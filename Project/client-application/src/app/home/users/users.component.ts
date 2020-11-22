import { Component, OnInit } from '@angular/core';
import { UserModel } from 'src/app/models/user.model';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadAllUsers();
  }

  users : UserModel[]

  loadAllUsers() {
    this.userService.getAllUsers().subscribe(
      (res:any) => {
        this.users = res;
        console.log(res);
      },
      err => {
        console.log(err);
      }
    );
  }
}
