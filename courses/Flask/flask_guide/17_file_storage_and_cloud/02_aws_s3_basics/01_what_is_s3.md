<!-- FILE: 17_file_storage_and_cloud/02_aws_s3_basics/01_what_is_s3.md -->

## Overview

Understand AWS S3 (Simple Storage Service) for cloud file storage.

## Prerequisites

- Basic understanding of file storage
- AWS account (optional for learning)

## Core Concepts

AWS S3 is an object storage service that provides scalability, data availability, security, and performance. Files are stored in "buckets" with globally unique names and accessed via URLs.

## Core Concepts

### What is S3?

Amazon S3 (Simple Storage Service) is object storage built to store and retrieve any amount of data from anywhere on the web. It's highly durable, available, and scalable.

### Key Terminology

| Term | Description |
|------|-------------|
| **Bucket** | Container for objects (like a top-level folder) |
| **Object** | File stored in S3 (any type of file) |
| **Key** | Unique identifier for each object in a bucket |
| **Region** | Physical location where S3 stores your data |
| **URL** | Access point: `https://bucket-name.s3.region.amazonaws.com/key` |

### Why Use S3?

- **Durability**: 99.999999999% (eleven 9s) durability
- **Scalability**: Store unlimited objects
- **Availability**: 99.99% availability SLA
- **Cost-effective**: Pay only for what you use
- **Security**: Built-in encryption and access controls

## Common Use Cases

1. **User-generated content**: Profile pictures, uploads
2. **Static assets**: CSS, JavaScript, images for websites
3. **Backups**: Database backups, file archives
4. **Media streaming**: Video, audio files
5. **Data lakes**: Big data analytics storage

## Quick Reference

| Feature | Description |
|---------|-------------|
| Bucket | Top-level container |
| Objects | Files (up to 5TB each) |
| Lifecycle | Auto-transition to cheaper storage |
| Versioning | Keep multiple versions |
| Replication | Cross-region redundancy |

## Next Steps

Continue to [02_setting_up_boto3.md](./02_setting_up_boto3.md) to learn how to connect Flask to S3.
