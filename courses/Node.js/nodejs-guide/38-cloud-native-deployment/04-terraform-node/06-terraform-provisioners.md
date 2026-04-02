# Terraform Provisioners

## What You'll Learn

- How to use provisioners for configuration
- Remote-exec and local-exec usage
- When to use vs when to avoid provisioners
- Alternative approaches

---

## Layer 1: Provisioners

### local-exec

```hcl
resource "null_resource" "deploy" {
  provisioner "local-exec" {
    command = "echo 'Deploying to ${aws_instance.web.public_ip}'"
  }
}

resource "aws_instance" "web" {
  # ... instance config ...
  
  provisioner "local-exec" {
    when    = destroy
    command = "echo 'Instance ${self.id} destroyed'"
  }
}
```

### remote-exec

```hcl
resource "aws_instance" "web" {
  # ... instance config ...
  
  connection {
    type        = "ssh"
    user        = "ubuntu"
    private_key = file("~/.ssh/id_rsa")
    host        = self.public_ip
  }
  
  provisioner "remote-exec" {
    inline = [
      "sudo apt-get update",
      "sudo apt-get install -y nodejs npm",
      "cd /opt && git clone https://github.com/myapp.git"
    ]
  }
}
```

---

## Next Steps

Continue to [Terraform Data](./07-terraform-data.md)