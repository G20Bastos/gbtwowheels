import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

interface User {
  firstName: string;
  lastName: string;
  cnpj: string;
  dateOfBirth: Date;
  categoryLicense: string;
  licenseNumber: number;
  imageFile: File | null;
  userEmail: string;
  userPassword: string;
}

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  user: User = {
    firstName: '',
    lastName: '',
    cnpj: '',
    dateOfBirth: new Date(),
    categoryLicense: '',
    licenseNumber: 0,
    imageFile: null,
    userEmail: '',
    userPassword: ''
  };

  constructor(private http: HttpClient, private router: Router) { }

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      this.user.imageFile = event.target.files[0];
    }
  }

  onSubmit() {
    const formData = new FormData();
    formData.append('firstName', this.user.firstName);
    formData.append('lastName', this.user.lastName);
    formData.append('cnpj', this.user.cnpj);

    const dateOfBirth = new Date(this.user.dateOfBirth);
    if (!isNaN(dateOfBirth.getTime())) {
      formData.append('dateOfBirth', dateOfBirth.toISOString());
    } else {
      console.error('Invalid date of birth');
      return;
    }

    formData.append('categoryLicense', this.user.categoryLicense);
    formData.append('licenseNumber', this.user.licenseNumber.toString());
    if (this.user.imageFile) {
      formData.append('imageFile', this.user.imageFile);
    }
    formData.append('userEmail', this.user.userEmail);
    formData.append('userPassword', this.user.userPassword);
    formData.append('levelId', '3');

    this.http.post('https://localhost:7296/api/user/AddUser', formData)
      .subscribe(response => {
        console.log('Succes', response);
        this.router.navigate(['/']);
      }, error => {
        console.error('Fail: ', error);
      });
  }

}
