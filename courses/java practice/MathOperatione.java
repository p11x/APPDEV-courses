public class MathOperatione {
    public static void main(String[] args) {
        

}

public int primeNumber(int n) {
            int count = 0;
            for (int i = 2; i <= n / 2; i++) {
                if (n % i == 0) {
                    count++;
                    break;
                }
            }
            if (count == 0 && n != 1) {
                return n;
            } else {
                return -1;
            } 
            
}   


public int palindrome(int n) {
    int r,sum = 0,temp ;
    
    temp=n;
    while(n>0){
        r=n%10;
        sum=(sum*10)+r;
        n=n/10;
    }
    if(temp==sum)
        return temp;
    else
        return -1;
}    


public int armstrong(int n) {
    int r,sum=0,temp;
    temp=n;
    while(n>0){
        r=n%10;
        sum=sum+(r*r*r);
        n=n/10;
    }
    if(temp==sum)
        return temp;
    else
        return -1;
    
}


public int factorial(int n) {
    int fact=1;
    for(int i=1;i<=n;i++){
        fact=fact*i;
    }
    return fact;
}

public int strong(int n) {
    int r,sum=0,temp;
    temp=n;
    while(n>0){
        r=n%10;
        sum=sum+factorial(r);
        n=n/10;
    }
    if(temp==sum)
        return temp;
    else
        return -1;


}

public int perfect(int n) {
    int sum=0;
    for(int i=1;i<n;i++){
        if(n%i==0){
            sum=sum+i;
        }
    }
    if(sum==n)
        return n;
    else
        return -1;
} 

public int sumofdigits(int n) {
    int sum=0;
    while(n>0){
        sum=sum+n%10;
        n=n/10;
    }
    return sum;
}


public int sumofdigitalcubes(int n) {
    int r,sum=0;
    while(n>0){
        r=n%10;
        sum=sum+(r*r*r);
        n=n/10;
    }
    return sum;
}

public int reverse(int n) {
    int r,rev=0;
    while(n>0){
        r=n%10;
        rev=(rev*10)+r;
        n=n/10;
    }
    return rev;
}

public int factorialofsumofdigits(int n) {
    int sum=0;
    while(n>0){
        sum=sum+n%10;
        n=n/10;
    }
    return factorial(sum);
} 

public int numberofdigits(int n) {
    int count=0;
    while(n>0){
        count++;
        n=n/10;
    }
    return count;
} 

public int numberoffactors(int n) {
    int count=0;
    for(int i=1;i<=n;i++){
        if(n%i==0){
            count++;
        }
    }
    return count;
}



public int sumoffactors(int n) {
    int sum=0;
    for(int i=1;i<n;i++){
        if(n%i==0){
            sum=sum+i;
        }
    }
    return sum;
}

}
