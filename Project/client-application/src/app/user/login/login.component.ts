import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from "@angular/forms";
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginModel } from 'src/app/models/login.model';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  formModel : LoginModel = {
    UserName : '',
    Password : ''
  };

  constructor(private userService: UserService, 
      private router: Router, 
      private toastr: ToastrService) { }

  ngOnInit(): void {
    if(this.isAuthenticated())
      this.router.navigateByUrl('/home');
  }

  onSubmit(form:NgForm) {
    this.userService.login(form.value).subscribe(
      (res:any) => {
        localStorage.setItem('token', res.token);
        this.toastr.success('Welcome ' + form.value.UserName + '!', 'Authentication Success')
        this.router.navigateByUrl('/home');
      },
      err => {
        if(err.status == 400)
          this.toastr.error('Incorrect username or password!', 'Authentication failed')
        else console.log(err);
      }
    );
  }

  isAuthenticated() {
    if(localStorage.getItem('token') != null)
      return true;
    else return false;
  }
}
