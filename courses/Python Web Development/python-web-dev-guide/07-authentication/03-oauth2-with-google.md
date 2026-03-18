# OAuth2 with Google

## What You'll Learn
- OAuth2 flow
- Using Authlib

## Prerequisites
- Completed JWT authentication

## OAuth2 Flow

1. User clicks "Login with Google"
2. Redirect to Google
3. User authorizes your app
4. Google redirects back with code
5. Exchange code for token

## Setup Google OAuth

```bash
pip install authlib
```

```python
from authlib.integrations.flask_client import OAuth

oauth = OAuth(app)
google = oauth.register('google',
    client_id='GOOGLE_CLIENT_ID',
    client_secret='GOOGLE_CLIENT_SECRET',
    server_metadata_url='https://accounts.google.com/.well-known/openid-configuration',
    client_kwargs={'scope': 'openid profile email'}
)

@app.route('/login')
def login():
    return google.authorize_redirect(redirect_uri=url_for('authorized', _external=True))

@app.route('/login/callback')
def authorized():
    token = google.authorize_access_token()
    user_info = google.get('userinfo').json()
    # Create or find user in database
    return jsonify(user_info)
```
