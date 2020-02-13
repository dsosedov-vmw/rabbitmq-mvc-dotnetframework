pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/framework/sdk:4.8'
            label 'windows'
        }
    }
    stages {
        stage('Build') {
            steps {
                echo 'Building...'
                //bat 'nuget restore src'
                //bat 'msbuild src'
            }
        }
        stage('Test') {
            steps {
                echo 'Testing...'
            }
        }
        stage('Publish') {
            steps {
                bat 'msbuild src /p:Configuration=Release /p:PublishProfile=FolderProfile /p:DeployOnBuild=true'
            }
        }
        stage('Deploy') {
            steps {
                pushToCloudFoundry(
                  target: 'api.run.pcfone.io',
                  organization: 'pivot-dsosedov',
                  cloudSpace: 'production',
                  credentialsId: 'cf_creds'
                 )
            }
        }
    }
    post {
        always {
            archiveArtifacts artifacts: 'manifest.yml', fingerprint: true
        }
    }
}
