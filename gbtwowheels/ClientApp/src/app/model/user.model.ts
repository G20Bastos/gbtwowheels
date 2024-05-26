export interface User {
  firstName: string;
  lastName: string;
  cnpj: string;
  dateOfBirth: Date;
  categoryLicense: string;
  licenseNumber: number;
  imageFile: File | null;
  userEmail: string;
  userPassword: string;
  levelId: number;
}
