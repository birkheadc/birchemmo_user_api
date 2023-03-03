# TODO
- Tie everything into the front-end
- Logging
- Ensure everything still works in production
  - Especially the email verification html template, the Path will probably be an issue
- Create a new class for incoming new user requests that does not include Role
  - CreateNewUser role should always be UNVALIDATED_USER, this request needs no validation
  - CreateNewAdmin role should always be ADMIN, this request is only callable by a super admin
- Build middleware that converts UserModels in response to UserViewModel .. remove calls to converter in controllers

- Changed email verification controller method VerifyEmail, make sure tests work