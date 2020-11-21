import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(public userService: UserService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.userService.formModel.reset();
  }

  onSubmit() {
    this.userService.register().subscribe(
      (res:any) =>  {
        if(res.succeeded) {
          this.userService.formModel.reset();
          this.toastr.success('Registration Completed Successfully', 'Registration Success');
        } else {
          res.errors.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                this.toastr.error('The Username is already taken', 'Registration Failure');
                break;
            
              default:
                this.toastr.error('Registration Failed! Connect our team to solve this trouble!', 'Registration Failure');
                break;
            }
          });
        }
      },
      err => {
        console.log(err);
      }
    )
  }
}
