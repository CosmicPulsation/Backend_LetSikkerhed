import React, { useState } from 'react';
import logo from './logo.svg';
import './App.css';

function App() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    setLoading(true);
    setTimeout(() => {
      setLoading(false);
      alert(`Logged in as ${username}, password: ${password}`);
    }, 800);
  };

  return (
    <div className="App login-bg">
      <div className="login-card">
        <div className="login-brand">
          <img src={logo} className="brand-logo" alt="logo" />
          <h1>Admin Panel</h1>
        </div>

        <form onSubmit={handleSubmit} className="login-form">
          <div className="input-group">
            <label>Username</label>
            <div className="input-with-icon">
              <span className="icon">👤</span>
              <input value={username} onChange={e => setUsername(e.target.value)} placeholder="Username" required />
            </div>
          </div>

          <div className="input-group">
            <label>Password</label>
            <div className="input-with-icon">
              <span className="icon">🔒</span>
              <input type={showPassword ? 'text' : 'password'} value={password} onChange={e => setPassword(e.target.value)} placeholder="Password" required />
              <button type="button" className="show-btn" onClick={() => setShowPassword(s => !s)}>{showPassword ? 'Hide' : 'Show'}</button>
            </div>
          </div>

          <div className="actions">
            <label className="remember"><input type="checkbox" /> Remember me</label>
            <button type="submit" className="primary" disabled={loading}>{loading ? 'Signing in...' : 'Sign in'}</button>
          </div>
        </form>

        <div className="login-footer">Need help? <a href="#">Reset password</a></div>
      </div>
    </div>
  );
}

export default App;
