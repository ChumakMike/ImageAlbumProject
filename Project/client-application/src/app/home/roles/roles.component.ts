import { Component, OnInit } from '@angular/core';
import { RoleModel } from 'src/app/models/role.model';
import { RoleService } from 'src/app/shared/role.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent implements OnInit {

  roles : RoleModel[];
  constructor(private roleService: RoleService) { }

  ngOnInit(): void {
    this.loadAllRoles();
  }

  loadAllRoles() {
    this.roleService.getAllRoles().subscribe(
      (res:any) => {
        this.roles = res;
      },
      err => {
        console.log(err);
      }
    )
  }

}
