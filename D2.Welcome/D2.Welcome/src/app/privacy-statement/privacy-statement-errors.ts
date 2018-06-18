export class ErrorMessage {
  constructor(
    public forControl: string,
    public forValidator: string,
    public message: string
  ) {}
}

export const ErrorMessages = [
  new ErrorMessage(
    'passwordOne',
    'pattern',
    'Das Passwort entspricht nicht den Kriterien.'),
  new ErrorMessage(
    'passwordOne',
    'required',
    'Vergeben Sie ein Passwort.'),
  new ErrorMessage(
    'passwordTwo',
    'required',
    'Die Wiederholung ist erforderlich.'),
  new ErrorMessage(
    'passwordTwo',
    'matching',
    'Die Passwörter stimmen nicht überein.')
  ];
