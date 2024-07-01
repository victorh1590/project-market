import './App.css'
import { SigninForm } from './components/SigninForm/SigninForm';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { UserInfo } from './components/UserInfo/UserInfo';
import { Header } from './components/Header/Header';
import { Grid } from './components/Grid/Grid'
import "./index.css"

function App() {
  const queryClient = new QueryClient();

  return (
    <Grid>
      <Header/>
      <QueryClientProvider client={queryClient}>
        <SigninForm />
        <br/>
        <UserInfo />
      </QueryClientProvider>
    </Grid> 
  );
}

export default App
