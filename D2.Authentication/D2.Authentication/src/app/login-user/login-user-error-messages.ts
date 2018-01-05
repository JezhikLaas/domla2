import { ErrorMessage } from '../shared/error-message';

export const ErrorMessages = [
  new ErrorMessage(
    'login',
    'required',
    'Geben Sie bitte ihren Nutzernamen an.'),
  new ErrorMessage(
    'password',
    'required',
    'Geben Sie bitte ihr Passwort an.')
];
