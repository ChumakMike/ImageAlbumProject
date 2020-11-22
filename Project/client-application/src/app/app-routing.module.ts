import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { CategoriesComponent } from './home/categories/categories.component';
import { HomeProfileComponent } from './home/home-profile/home-profile.component';
import { HomeComponent } from './home/home.component';
import { RolesComponent } from './home/roles/roles.component';
import { UsersComponent } from './home/users/users.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { UserComponent } from './user/user.component';

const routes: Routes = [
  { path:'', redirectTo: '/user/login', pathMatch:'full'},
  {
  path: 'user', component: UserComponent,
  children: [
    { path: 'registration', component: RegistrationComponent},
    { path: 'login', component: LoginComponent }
    ]
  },
  { 
    path: 'home', component: HomeComponent, canActivate:[AuthGuard],
    children: [
      { 
        path: 'users', component: UsersComponent, canActivate:[AuthGuard], 
        data:{permittedRoles: ['Admin']}
      },
      {
        path: 'home-profile', component: HomeProfileComponent, canActivate:[AuthGuard]
      },
      {
        path: 'roles', component: RolesComponent, canActivate:[AuthGuard],
        data:{permittedRoles: ['Admin']}
      },
      {
        path: 'categories', component: CategoriesComponent, canActivate:[AuthGuard],
        data:{permittedRoles: ['Admin', 'Manager']}
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
