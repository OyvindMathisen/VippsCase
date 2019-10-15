import { Component, OnInit, Output } from '@angular/core';
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { LoginService } from "../../services/login.service";
import { Login } from "../../shared/models/login.model";
import { Router } from '@angular/router';


@Component({
  selector: 'app-login-module',
  templateUrl: './login-module.component.html',
  styleUrls: ['./login-module.component.scss']
})

@Injectable()
export class LoginModuleComponent implements OnInit {

  @Output() errorMessage: string;

  login = {} as Login;
  loginError = false; 

  constructor(private http: HttpClient,
    private loginService: LoginService,
    private router: Router) { }

  ngOnInit() {
  }

  loginClicked(email, pwd){
    console.log(email);
    console.log(pwd);
    
    this.login = {
      email: email,
      password: pwd
    }
    
    
    this.loginService.login(this.login).subscribe(
      data => {
        this.router.navigate(['/purchase']);
        console.log(data);
        this.loginError = false;
      },
      error => {
        console.log(error);
        this.loginError = true;
        this.errorMessage = error.statusText + ": Invalid username or password!";
        
      }
    );

    // Returning false to prevent the page from updating
    return false;
  }
}
