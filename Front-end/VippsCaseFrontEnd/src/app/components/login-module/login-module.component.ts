import { Component, OnInit, Output } from '@angular/core';
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { LoginService } from "../../services/login.service";
import { Login } from "../../shared/models/login.model";
import { Router } from '@angular/router';
import * as moment from "moment";


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
        this.setSession(data.token);
        this.loginError = false;
        console.log(localStorage.getItem("id_token"));
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

  private setSession(authResult) {
    const expiresAt = moment().add(authResult.expiresIn,'second');

    localStorage.setItem('id_token', authResult);
    //localStorage.setItem("expires_at", JSON.stringify(expiresAt.valueOf()) );
}  
}
